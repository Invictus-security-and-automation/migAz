﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Core
{
    public class TreeView
    {

        public static TreeNode GetDataCenterTreeViewNode(TreeNode subscriptionNode, string dataCenter, string containerName)
        {
            TreeNode dataCenterNode = null;

            foreach (TreeNode treeNode in subscriptionNode.Nodes)
            {
                if (treeNode.Text == dataCenter && treeNode.Tag.ToString() == "DataCenter")
                {
                    dataCenterNode = treeNode;

                    foreach (TreeNode dataCenterContainerNode in treeNode.Nodes)
                    {
                        if (dataCenterContainerNode.Text == containerName)
                            return dataCenterContainerNode;
                    }
                }
            }

            if (dataCenterNode == null)
            {
                dataCenterNode = new TreeNode(dataCenter);
                dataCenterNode.Tag = "DataCenter";
                subscriptionNode.Nodes.Add(dataCenterNode);
                dataCenterNode.Expand();
            }

            if (containerName == "Virtual Networks")
            {
                TreeNode tnVirtualNetworks = new TreeNode("Virtual Networks");
                dataCenterNode.Nodes.Add(tnVirtualNetworks);
                tnVirtualNetworks.Expand();

                return tnVirtualNetworks;
            }
            else if (containerName == "Storage Accounts")
            {
                TreeNode tnStorageAccounts = new TreeNode("Storage Accounts");
                dataCenterNode.Nodes.Add(tnStorageAccounts);
                tnStorageAccounts.Expand();

                return tnStorageAccounts;
            }
            else if (containerName == "Cloud Services")
            {
                TreeNode tnCloudServicesNode = new TreeNode("Cloud Services");
                dataCenterNode.Nodes.Add(tnCloudServicesNode);
                tnCloudServicesNode.Expand();

                return tnCloudServicesNode;
            }
            else if (containerName == "Network Security Groups")
            {
                TreeNode tnNetworkSecurityGroupsNode = new TreeNode("Network Security Groups");
                dataCenterNode.Nodes.Add(tnNetworkSecurityGroupsNode);
                tnNetworkSecurityGroupsNode.Expand();

                return tnNetworkSecurityGroupsNode;
            }

            return null;
        }

        public static TreeNode GetDataCenterTreeViewNode(TreeNode subscriptionNode, object location, string v)
        {
            throw new NotImplementedException();
        }
    }
}
