import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Configuration } from '../../models/configuration';
import { ConfigurationService } from '../../services/configuration.service';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

@Component({
  selector: 'app-configuration',
  imports: [MatFormFieldModule, MatInputModule, FormsModule, MatCheckboxModule, MatButtonModule, MatSlideToggleModule],
  templateUrl: './configuration.component.html',
  styleUrl: './configuration.component.css'
})
export class ConfigurationComponent implements OnInit {
  public configuration: Configuration = null as any;
  constructor(private configService: ConfigurationService){
   
  }

  ngOnInit(): void {
    this.configService.getConfiguration().subscribe(cfg => {
    this.configuration = cfg;
    console.log("Configuration loaded:", this.configuration);
  });
  }

  isNotificationEnabled(): boolean {
    return this.configuration && this.configuration.enableNotifications;
  }

  isAutomationEnabled(): boolean {
    return this.configuration && this.configuration.enableAutomation;
  }

  saveConfiguration() {
    this.configService.setConfiguration(this.configuration).subscribe(
      response => {
        console.log("Configuration saved successfully:", response);
      },
      error => {
        console.error("Error saving configuration:", error);
      }
    );
  }

}
