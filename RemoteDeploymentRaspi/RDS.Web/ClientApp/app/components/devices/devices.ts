import { HttpClient } from 'aurelia-fetch-client';

import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class Devices {
    public mode: string;
    public mydevices: DeviceIdentity[];
    public node: DeviceIdentity;
    public UpdatedNode: DeviceIdentity;

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
            lat: 0
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

    Update(sel) {
        this.UpdatedNode = sel;
        this.mode = 'update';
    }

    Delete(sel) {
        this.UpdatedNode = sel;
        this.mode = 'delete';

    }

    addNew() {
        this.mode = 'add';
    }

    back() {
        this.mode = 'list';
    }

    saveData() {


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

    }
    updateData() {


        this.http.fetch('/api/Device/' + this.UpdatedNode.id, {
            method: "PUT",
            body: JSON.stringify(this.UpdatedNode),
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

    }
    deleteData() {
        this.http.fetch('/api/Device/' + this.UpdatedNode.id, {
            method: "DELETE"
        })
            .then(response => response.json())
            .then(data => {
                console.log(data);
                this.getData();
            });
        this.mode = 'list';

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
