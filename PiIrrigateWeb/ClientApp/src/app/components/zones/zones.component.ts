import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTreeModule } from '@angular/material/tree';
import { MatIconModule } from '@angular/material/icon';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { ZonesService } from '../../services/zones.service';
import { ZoneData, Device } from '../../models/zone';

interface FlatNode {
  expandable: boolean;
  name: string;
  level: number;
  mac?: string;
}

@Component({
  selector: 'app-zones',
  standalone: true,
  imports: [CommonModule, MatTreeModule, MatIconModule],
  providers: [ZonesService],
  template: `
    <div class="header">
      <h2 class="title">Zones & Devices</h2>
    </div>

    <mat-tree [dataSource]="dataSource" [treeControl]="treeControl" class="mat-elevation-z">
      <!-- Zone node -->
      <mat-tree-node *matTreeNodeDef="let node; when: hasChild" matTreeNodeToggle>
        <li class="mat-tree-node">
          <div class="icon-container">
          <strong>{{node.name}}</strong>
          <div class="spacer"></div>
          <button mat-icon-button matTreeNodeToggle [attr.aria-label]="'toggle ' + node.name">
            <mat-icon>
              {{treeControl.isExpanded(node) ? 'expand_more' : 'chevron_right'}}
            </mat-icon>
          </button>
          </div>
        </li>
      </mat-tree-node>
      <!-- Device node -->
      <mat-tree-node *matTreeNodeDef="let node">
        <li class="mat-tree-node" style="padding-left: 40px;">
          <mat-icon color="primary" style="vertical-align: middle;">memory</mat-icon>
          {{node.name}} <span style="color: white;">({{node.mac}})</span>
        </li>
      </mat-tree-node>
    </mat-tree>
  `,
  styles: `
    mat-tree {
      width: 100%;
      border-radius: 8px;
      margin-top: 16px;
      padding: 8px 0;
    }
    .mat-tree-node {
      padding: 8px;
      display: flex;
      align-items: center;
      font-size: 16px;
    }
    .icon-container {
      display: flex;
      align-items: center;
      flex-grow: 1;
    }
    .spacer{
      flex-grow: 1;
    }
    .divider{
      border-bottom: 1px solid #e0e0e0;
      margin: 8px 0;
    }
  `
})
export class ZonesComponent {
  private _transformer = (node: ZoneData | Device, level: number) => ({
    expandable: !!(node as ZoneData).devices && (node as ZoneData).devices!.length > 0,
    name: (node as ZoneData).name || (node as Device).name || (node as Device).mac,
    mac: (node as Device).mac,
    level: level,
  });

  treeControl = new FlatTreeControl<FlatNode>(
    node => node.level,
    node => node.expandable
  );

  treeFlattener = new MatTreeFlattener<ZoneData | Device, FlatNode>(
    this._transformer,
    node => node.level,
    node => node.expandable,
    node => (node as ZoneData).devices
  );

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(private zonesService: ZonesService) {
    this.zonesService.updateZones();
    setTimeout(() => {
      // zonesWithDevices: ZoneData[]
      this.dataSource.data = this.zonesService.zonesWithDevices || [];
    }, 500);
  }

  hasChild = (_: number, node: FlatNode) => node.expandable;
}
