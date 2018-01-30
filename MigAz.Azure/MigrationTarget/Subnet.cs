﻿using MigAz.Core;
using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class Subnet : IMigrationTarget, IMigrationSubnet
    {
        private AzureContext _AzureContext = null;
        private ISubnet _SourceSubnet;
        private String _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;
        private MigrationTarget.VirtualNetwork _ParentVirtualNetwork;

        private Subnet() { }

        public Subnet(MigrationTarget.VirtualNetwork parentVirtualNetwork, ISubnet source, List<NetworkSecurityGroup> networkSecurityGroups, List<RouteTable> routeTables, TargetSettings targetSettings)
        {
            _ParentVirtualNetwork = parentVirtualNetwork;
            _SourceSubnet = source;

            if (source.GetType() == typeof(Asm.Subnet))
            {
                Asm.Subnet asmSubnet = (Asm.Subnet)source;

                if (asmSubnet.NetworkSecurityGroup != null)
                {
                    this.NetworkSecurityGroup = SeekNetworkSecurityGroup(networkSecurityGroups, asmSubnet.NetworkSecurityGroup.ToString());
                }

                if (asmSubnet.RouteTable != null)
                {
                    this.RouteTable = SeekRouteTable(routeTables, asmSubnet.RouteTable.ToString());
                }
            }
            else if (source.GetType() == typeof(Arm.Subnet))
            {
                Arm.Subnet armSubnet = (Arm.Subnet)source;

                if (armSubnet.NetworkSecurityGroup != null)
                {
                    this.NetworkSecurityGroup = SeekNetworkSecurityGroup(networkSecurityGroups, armSubnet.NetworkSecurityGroup.ToString());
                }

                if (armSubnet.RouteTable != null)
                {
                    this.RouteTable = SeekRouteTable(routeTables, armSubnet.RouteTable.ToString());
                }

            }

            this.AddressPrefix = source.AddressPrefix;
            this.SetTargetName(source.Name, targetSettings);
        }

        public Subnet(VirtualNetwork parentVirtualNetwork, ISubnet sourceSubnet, TargetSettings targetSettings)
        {
            this._ParentVirtualNetwork = parentVirtualNetwork;
            this._SourceSubnet = sourceSubnet;
            this.SetTargetName(sourceSubnet.Name, targetSettings);
            this.AddressPrefix = sourceSubnet.AddressPrefix;
        }

        private NetworkSecurityGroup SeekNetworkSecurityGroup(List<NetworkSecurityGroup> networkSecurityGroups, string sourceName)
        {
            if (networkSecurityGroups == null || sourceName == null)
                return null;

            foreach (NetworkSecurityGroup networkSecurityGroup in networkSecurityGroups)
            {
                if (networkSecurityGroup.SourceName == sourceName)
                    return networkSecurityGroup;
            }

            return null;
        }
        private RouteTable SeekRouteTable(List<RouteTable> routeTables, string sourceName)
        {
            if (routeTables == null || sourceName == null)
                return null;

            foreach (RouteTable routeTable in routeTables)
            {
                if (routeTable.SourceName == sourceName)
                    return routeTable;
            }

            return null;
        }

        public String AddressPrefix { get; set; }

        public ISubnet SourceSubnet
        {
            get { return _SourceSubnet; }
        }

        public String SourceName
        {
            get
            {
                if (this.SourceSubnet == null)
                    return String.Empty;
                else
                    return this.SourceSubnet.ToString();
            }
        }

        public MigrationTarget.VirtualNetwork ParentVirtualNetwork
        {
            get { return _ParentVirtualNetwork; }
        }

        public string TargetId
        {
            get { return "[concat(" + ArmConst.ResourceGroupId + ", '" + ArmConst.ProviderVirtualNetwork + this.ParentVirtualNetwork.ToString() + "/subnets/" + this.TargetName + "')]"; }
        }

        public RouteTable RouteTable { get; set;  }
        public NetworkSecurityGroup NetworkSecurityGroup { get; set; }

        public bool IsGatewaySubnet
        {
            get { return this.TargetName == ArmConst.GatewaySubnetName; }
        }

        public string TargetName
        {
            get { return _TargetName; }
        }

        public string TargetNameResult
        {
            get { return _TargetNameResult; }
        }

        public void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            _TargetName = targetName.Trim().Replace(" ", String.Empty).Replace("-", String.Empty);
            _TargetNameResult = _TargetName;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
