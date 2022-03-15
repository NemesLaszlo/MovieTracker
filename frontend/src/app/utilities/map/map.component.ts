import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { coordinatesMap, coordinatesMapWithMessage } from './coordinate';
import {Marker, marker, tileLayer, latLng, LeafletMouseEvent, icon} from 'leaflet'

const iconRetinaUrl = 'assets/marker-icon-2x.png';
const iconUrl = 'assets/marker-icon.png';
const shadowUrl = 'assets/marker-shadow.png';
const iconDefault = icon({
  iconRetinaUrl,
  iconUrl,
  shadowUrl,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  tooltipAnchor: [16, -28],
  shadowSize: [41, 41]
});
Marker.prototype.options.icon = iconDefault;

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit {
  layers: Marker<any>[] = [];

  @Input() initialCoordinates: coordinatesMapWithMessage[] = [];
  @Input() editMode: boolean = true;
  @Output() onSelectedLocation = new EventEmitter<coordinatesMap>();

  constructor() { }

  ngOnInit(): void {
    // this.layers = this.initialCoordinates.map(value => marker([value.latitude, value.longitude]));
    this.layers = this.initialCoordinates.map((value) => {
      const m = marker([value.latitude, value.longitude]);
      if (value.message){
        m.bindPopup(value.message, {autoClose: false, autoPan: false});
      }
      return m;
    });
  }

  options = {
    layers: [
      tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { 
        maxZoom: 18, 
      attribution: 'Movie Tracker' })
    ],
    zoom: 14,
    center: latLng(18.473564631048617,  -69.93999481201173)
  };

  handleMapClick(event: LeafletMouseEvent){
    if(this.editMode) {
      const latitude = event.latlng.lat;
      const longitude = event.latlng.lng;
      console.log({latitude, longitude});
      this.layers = [];
      this.layers.push(marker([latitude, longitude]));
      this.onSelectedLocation.emit({latitude, longitude});
    }
  }

}
