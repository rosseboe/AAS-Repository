﻿
namespace AdminShell
{
    using Aml.Engine.CAEX;
    using Kusto.Cloud.Platform.Communication;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using static AAS_Repository.Pages.TreePage;

    public class VisualTreeBuilderService
    {
        private readonly UANodesetViewer _viewer;
        private readonly AASXPackageService _packageService;
        private readonly ILogger _logger;
        private readonly CarbonReportingService _carbonReporting;
        private readonly ProductCarbonFootprintService _pcf;

        public static event EventHandler NewDataAvailable;

        public VisualTreeBuilderService(ILoggerFactory logger, UANodesetViewer viewer, AASXPackageService packages, CarbonReportingService carbonReporting, ProductCarbonFootprintService pcf)
        {
            _viewer = viewer;
            _packageService = packages;
            _carbonReporting = carbonReporting;
            _pcf = pcf;
            _logger = logger.CreateLogger("VisualTreeBuilderService");
        }

        public static void SignalNewData(TreeUpdateMode mode)
        {
            NewDataAvailable?.Invoke(mode, EventArgs.Empty);
        }

        public List<TreeNodeData> BuildTree()
        {
            List<TreeNodeData> viewItems = new List<TreeNodeData>();

            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                TreeNodeData root = new TreeNodeData();
                root.EnvKey = package.Key;
                root.Text = package.Value.AssetAdministrationShells[0].IdShort;
                root.Tag = package.Value.AssetAdministrationShells[0];
                CreateViewFromAASEnv(root, package.Key, package.Value);
                viewItems.Add(root);
            }

            return viewItems;
        }

        private void CreateViewFromAASEnv(TreeNodeData root, string key, AssetAdministrationShellEnvironment aasEnv)
        {
            List<TreeNodeData> subModelTreeNodeDataList = new List<TreeNodeData>();
            foreach (Submodel subModel in aasEnv.Submodels)
            {
                if (subModel != null && subModel.IdShort != null)
                {
                    TreeNodeData subModelTreeNodeData = new()
                    {
                        EnvKey = key,
                        Text = subModel.IdShort,
                        Tag = subModel
                    };

                    subModelTreeNodeDataList.Add(subModelTreeNodeData);
                    CreateViewFromSubModel(subModelTreeNodeData, key, subModel);
                }
            }

            root.Children = subModelTreeNodeDataList;

            foreach (TreeNodeData nodeData in subModelTreeNodeDataList)
            {
                nodeData.Parent = root;
            }
        }

        private void CreateViewFromSubModel(TreeNodeData rootItem, string key, Submodel subModel)
        {
            List<TreeNodeData> subModelElementTreeNodeDataList = new List<TreeNodeData>();
            foreach (SubmodelElementWrapper smew in subModel.SubmodelElements)
            {
                TreeNodeData subModelElementTreeNodeData = new()
                {
                    EnvKey = key,
                    Text = smew.SubmodelElement.IdShort,
                    Tag = smew.SubmodelElement
                };

                subModelElementTreeNodeDataList.Add(subModelElementTreeNodeData);

                if (smew.SubmodelElement is SubmodelElementCollection)
                {
                    SubmodelElementCollection submodelElementCollection = smew.SubmodelElement as SubmodelElementCollection;
                    CreateViewFromSubModelElementCollection(subModelElementTreeNodeData, key, submodelElementCollection);
                }

                if (smew.SubmodelElement is Operation)
                {
                    Operation operation = smew.SubmodelElement as Operation;
                    CreateViewFromOperation(subModelElementTreeNodeData, key, operation);
                }

                if (smew.SubmodelElement is Entity)
                {
                    Entity entity = smew.SubmodelElement as Entity;
                    CreateViewFromEntity(subModelElementTreeNodeData, key, entity);
                }
            }

            rootItem.Children = subModelElementTreeNodeDataList;

            foreach (TreeNodeData nodeData in subModelElementTreeNodeDataList)
            {
                nodeData.Parent = rootItem;
            }
        }

        private void CreateViewFromSubModelElementCollection(TreeNodeData rootItem, string key, SubmodelElementCollection subModelElementCollection)
        {
            List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();
            foreach (SubmodelElementWrapper smew in subModelElementCollection.Value)
            {
                if (smew != null && smew != null)
                {
                    TreeNodeData smeItem = new()
                    {
                        EnvKey = key,
                        Text = smew.SubmodelElement.IdShort,
                        Tag = smew.SubmodelElement
                    };

                    treeNodeDataList.Add(smeItem);

                    if (smew.SubmodelElement is SubmodelElementCollection)
                    {
                        SubmodelElementCollection smecNext = smew.SubmodelElement as SubmodelElementCollection;
                        CreateViewFromSubModelElementCollection(smeItem, key, smecNext);
                    }

                    if (smew.SubmodelElement is Operation)
                    {
                        Operation operation = smew.SubmodelElement as Operation;
                        CreateViewFromOperation(smeItem, key, operation);
                    }

                    if (smew.SubmodelElement is Entity)
                    {
                        Entity entity = smew.SubmodelElement as Entity;
                        CreateViewFromEntity(smeItem, key, entity);
                    }

                    if (smew.SubmodelElement.IdShort == "NODESET2_XML"
                    && Uri.IsWellFormedUriString(((File)smew.SubmodelElement).Value, UriKind.Absolute))
                    {
                        CreateViewFromAdminShellNodeset(smeItem, key, new Uri(((File)smew.SubmodelElement).Value));
                    }

                    if (smew.SubmodelElement.IdShort == "CAEX")
                    {
                        CreateViewFromAMLCAEXFile(smeItem, key, ((File)smew.SubmodelElement).Value);
                    }
                }
            }

            rootItem.Children = treeNodeDataList;

            foreach (TreeNodeData nodeData in treeNodeDataList)
            {
                nodeData.Parent = rootItem;
            }
        }

        private void CreateViewFromAMLCAEXFile(TreeNodeData rootItem, string key, string filename)
        {
            try
            {
                byte[] fileContents = _packageService.GetFileContentsFromPackagePart(key, filename);
                CAEXDocument doc = CAEXDocument.LoadFromBinary(fileContents);
                List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();

                foreach (var instanceHirarchy in doc.CAEXFile.InstanceHierarchy)
                {
                    TreeNodeData smeItem = new()
                    {
                        EnvKey = key,
                        Text = instanceHirarchy.ID,
                        Type = "AML",
                        Tag = new SubmodelElement() { IdShort = instanceHirarchy.Name },
                        Children = new List<TreeNodeData>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (var internalElement in instanceHirarchy.InternalElement)
                    {
                        CreateViewFromInternalElement(smeItem, (List<TreeNodeData>)smeItem.Children, key, internalElement);
                    }
                }

                foreach (var roleclassLib in doc.CAEXFile.RoleClassLib)
                {
                    TreeNodeData smeItem = new()
                    {
                        EnvKey = key,
                        Text = roleclassLib.ID,
                        Type = "AML",
                        Tag = new SubmodelElement() { IdShort = roleclassLib.Name },
                        Children = new List<TreeNodeData>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (RoleFamilyType roleClass in roleclassLib.RoleClass)
                    {
                        CreateViewFromRoleClasses(smeItem, (List<TreeNodeData>)smeItem.Children, key, roleClass);
                    }
                }

                foreach (var systemUnitClassLib in doc.CAEXFile.SystemUnitClassLib)
                {
                    TreeNodeData smeItem = new()
                    {
                        EnvKey = key,
                        Text = systemUnitClassLib.ID,
                        Type = "AML",
                        Tag = new SubmodelElement() { IdShort = systemUnitClassLib.Name },
                        Children = new List<TreeNodeData>()
                    };

                    treeNodeDataList.Add(smeItem);

                    foreach (SystemUnitFamilyType systemUnitClass in systemUnitClassLib.SystemUnitClass)
                    {
                        CreateViewFromSystemUnitClasses(smeItem, (List<TreeNodeData>)smeItem.Children, key, systemUnitClass);
                    }
                }

                rootItem.Children = treeNodeDataList;

                foreach (TreeNodeData nodeData in treeNodeDataList)
                {
                    nodeData.Parent = rootItem;
                }
            }
            catch (Exception ex)
            {
                // ignore this node
                _logger.LogError(ex, ex.Message);
            }
        }

        private void CreateViewFromInternalElement(TreeNodeData rootItem, List<TreeNodeData> rootItemChildren, string key, InternalElementType internalElement)
        {
            TreeNodeData smeItem = new()
            {
                EnvKey = key,
                Text = internalElement.ID,
                Type = "AML",
                Tag = new SubmodelElement() { IdShort = internalElement.Name },
                Parent = rootItem,
                Children = new List<TreeNodeData>()
            };

            rootItemChildren.Add(smeItem);

            foreach (InternalElementType childInternalElement in internalElement.InternalElement)
            {
                CreateViewFromInternalElement(smeItem, (List<TreeNodeData>)smeItem.Children, key, childInternalElement);
            }
        }

        private void CreateViewFromRoleClasses(TreeNodeData rootItem, List<TreeNodeData> rootItemChildren, string key, RoleFamilyType roleClass)
        {
            TreeNodeData smeItem = new()
            {
                EnvKey = key,
                Text = roleClass.ID,
                Type = "AML",
                Tag = new SubmodelElement() { IdShort = roleClass.Name },
                Parent = rootItem,
                Children = new List<TreeNodeData>()
            };

            rootItemChildren.Add(smeItem);

            foreach (RoleFamilyType childRoleClass in roleClass.RoleClass)
            {
                CreateViewFromRoleClasses(smeItem, (List<TreeNodeData>)smeItem.Children, key, childRoleClass);
            }
        }

        private void CreateViewFromSystemUnitClasses(TreeNodeData rootItem, List<TreeNodeData> rootItemChildren, string key, SystemUnitFamilyType systemUnitClass)
        {
            TreeNodeData smeItem = new()
            {
                EnvKey = key,
                Text = systemUnitClass.ID,
                Type = "AML",
                Tag = new SubmodelElement() { IdShort = systemUnitClass.Name },
                Parent = rootItem,
                Children = new List<TreeNodeData>()
            };

            rootItemChildren.Add(smeItem);

            foreach (InternalElementType childInternalElement in systemUnitClass.InternalElement)
            {
                CreateViewFromInternalElement(smeItem, (List<TreeNodeData>)smeItem.Children, key, childInternalElement);
            }

            foreach (SystemUnitFamilyType childSystemUnitClass in systemUnitClass.SystemUnitClass)
            {
                CreateViewFromSystemUnitClasses(smeItem, (List<TreeNodeData>)smeItem.Children, key, childSystemUnitClass);
            }
        }

        private void CreateViewFromAdminShellNodeset(TreeNodeData rootItem, string key, Uri uri)
        {
            try
            {
                _viewer.Login(uri.AbsoluteUri, Environment.GetEnvironmentVariable("UACLUsername"), Environment.GetEnvironmentVariable("UACLPassword"));

                NodesetViewerNode rootNode = _viewer.GetRootNode().GetAwaiter().GetResult();
                if (rootNode != null && rootNode.Children)
                {
                    CreateViewFromUANode(rootItem, _viewer, key, rootNode);
                }
            }
            catch (Exception ex)
            {
                // ignore this part of the AAS
                _logger.LogError(ex, ex.Message);
            }
        }

        private void CreateViewFromUANode(TreeNodeData rootItem, UANodesetViewer viewer, string key, NodesetViewerNode rootNode)
        {
            try
            {
                List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();
                List<NodesetViewerNode> children = viewer.GetChildren(rootNode.Id).GetAwaiter().GetResult();
                foreach (NodesetViewerNode node in children)
                {
                    TreeNodeData smeItem = new TreeNodeData
                    {
                        EnvKey = key,
                        Text = node.Text,
                        Type = "UANode",
                        Tag = new SubmodelElement() { IdShort = node.Text }
                    };

                    treeNodeDataList.Add(smeItem);

                    if (node.Children)
                    {
                        CreateViewFromUANode(smeItem, viewer, key, node);
                    }
                }

                rootItem.Children = treeNodeDataList;

                foreach (TreeNodeData nodeData in treeNodeDataList)
                {
                    nodeData.Parent = rootItem;
                }
            }
            catch (Exception ex)
            {
                // ignore this node
                _logger.LogError(ex, ex.Message);
            }
        }

        private void CreateViewFromOperation(TreeNodeData rootItem, string key, Operation operation)
        {
            List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();
            foreach (OperationVariable v in operation.InputVariables)
            {
                TreeNodeData smeItem = new TreeNodeData
                {
                    EnvKey = key,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "In",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            foreach (OperationVariable v in operation.OutputVariables)
            {
                TreeNodeData smeItem = new TreeNodeData
                {
                    EnvKey = key,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "Out",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            foreach (OperationVariable v in operation.InoutputVariables)
            {
                TreeNodeData smeItem = new TreeNodeData
                {
                    EnvKey = key,
                    Text = v.Value.SubmodelElement.IdShort,
                    Type = "InOut",
                    Tag = v.Value.SubmodelElement
                };

                treeNodeDataList.Add(smeItem);
            }

            rootItem.Children = treeNodeDataList;

            foreach (TreeNodeData nodeData in treeNodeDataList)
            {
                nodeData.Parent = rootItem;
            }
        }

        private void CreateViewFromEntity(TreeNodeData rootItem, string key, Entity entity)
        {
            List<TreeNodeData> treeNodeDataList = new List<TreeNodeData>();
            foreach (SubmodelElementWrapper statement in entity.Statements)
            {
                if (statement != null && statement != null)
                {
                    TreeNodeData smeItem = new TreeNodeData
                    {
                        EnvKey = key,
                        Text = statement.SubmodelElement.IdShort,
                        Type = "In",
                        Tag = statement.SubmodelElement
                    };

                    treeNodeDataList.Add(smeItem);
                }
            }

            rootItem.Children = treeNodeDataList;

            foreach (TreeNodeData nodeData in treeNodeDataList)
            {
                nodeData.Parent = rootItem;
            }
        }
    }
}
