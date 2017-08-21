import { HttpClient } from 'aurelia-fetch-client';

import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class UpgradeFirmware {
    public mode: string;
    public mydevices: DeviceIdentity[];
    public node: DeviceIdentity;

    http: HttpClient;
    constructor(http: HttpClient) {
        this.http = http;
        this.mode = 'list';
        this.node = {
            id: 0,
            name: '',
            desc: '',
            ipAddress: '',
            userName: '',
            password: '',
            firmwareVersion: '',
            location: '',
            long: 0,
            lat: 0,
            isSelect: false

        };
        this.getData();

    }

    getData() {
        this.http.fetch('/api/Device/GetDevices')
            .then(result => result.json() as Promise<DeviceIdentity[]>)
            .then(data => {
                this.mydevices = data;
            });

    }

    

    Execute() {
        alert('execute clicked');
        /*

        this.http.fetch('/api/Device', {
            method: "POST",
            body: JSON.stringify(this.node),
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            }

        })
            .then(response => response.json())
            .then(data => {
                this.getData();
                console.log(data);
            });
        this.mode = 'list';
        */
    }
    
}

interface DeviceIdentity {
    id: number;
    name: string;
    desc: string;
    ipAddress: string;
    userName: string;
    password: string;
    firmwareVersion: string;
    location: string;
    long: number;
    lat: number;
    isSelect: boolean;
}
