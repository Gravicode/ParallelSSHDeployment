import { HttpClient } from 'aurelia-fetch-client';

import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class UpdateHistorys {
    public mode: string;
    public myUpdateHistorys: UpdateHistory[];
    public node: UpdateHistory;
    public UpdatedNode: UpdateHistory;

    http: HttpClient;
    constructor(http: HttpClient) {
        this.http = http;
        this.mode = 'list';
       
        this.getData();

    }

    getData() {
        this.http.fetch('/api/UpdateHistory/GetUpdateHistorys')
            .then(result => result.json() as Promise<UpdateHistory[]>)
            .then(data => {
                this.myUpdateHistorys = data;
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


        this.http.fetch('/api/UpdateHistory', {
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


        this.http.fetch('/api/UpdateHistory/' + this.UpdatedNode.id, {
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
        this.http.fetch('/api/UpdateHistory/' + this.UpdatedNode.id, {
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

interface UpdateHistory {
    id: number,
    updateDate: Date,
    updateBy: string,
    firmwareVersion: string,
    deviceId: number,
    deviceName: string
}
