import { HttpClient } from 'aurelia-fetch-client';

import { inject } from 'aurelia-framework';

@inject(HttpClient)
export class UpdateQues {
    public mode: string;
    public myUpdateQues: UpdateQue[];
    public node: UpdateQue;
    public UpdatedNode: UpdateQue;

    http: HttpClient;
    constructor(http: HttpClient) {
        this.http = http;
        this.mode = 'list';
       
        this.getData();

    }

    getData() {
        this.http.fetch('/api/UpdateQue/GetUpdateQues')
            .then(result => result.json() as Promise<UpdateQue[]>)
            .then(data => {
                this.myUpdateQues = data;
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


        this.http.fetch('/api/UpdateQue', {
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


        this.http.fetch('/api/UpdateQue/' + this.UpdatedNode.id, {
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
        this.http.fetch('/api/UpdateQue/' + this.UpdatedNode.id, {
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

interface UpdateQue {
    id: number,
    createdDate: Date,
    updatedDate: Date,
    attempt: number,
    deviceId: number,
    deviceName: string,
    firmwareId: number,
    firmwareVersion: string,
    firmwareUrl:string,
    status: string,
    updatedBy: string
}
