import { HttpClient } from 'aurelia-fetch-client';
import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class Devices {
    public mydevices: DeviceIdentity[];

    constructor(http: HttpClient) {
        http.fetch('/api/Device/GetDevices')
            .then(result => result.json() as Promise<DeviceIdentity[]>)
            .then(data => {
                this.mydevices = data;
            });
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
}
