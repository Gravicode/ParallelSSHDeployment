import { HttpClient } from 'aurelia-fetch-client';

import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class FirmwareInfos {
    public mode: string;
    public myFirmwareInfos: FirmwareInfo[];
    public node: FirmwareInfo;
    public UpdatedNode: FirmwareInfo;

    http: HttpClient;
    constructor(http: HttpClient) {
        this.http = http;
        this.mode = 'list';
       
        this.getData();

    }

    getData() {
        this.http.fetch('/api/FirmwareInfo/GetFirmwareInfos')
            .then(result => result.json() as Promise<FirmwareInfo[]>)
            .then(data => {
                this.myFirmwareInfos = data;
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


        this.http.fetch('/api/FirmwareInfo', {
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


        this.http.fetch('/api/FirmwareInfo/' + this.UpdatedNode.id, {
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
        this.http.fetch('/api/FirmwareInfo/' + this.UpdatedNode.id, {
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

interface FirmwareInfo {
    id: number,
    name: string,
    version: string
    releaseDate: Date,
    releaseBy: string,
    remark: string
}
