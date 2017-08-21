import { Aurelia, PLATFORM } from 'aurelia-framework';
import { Router, RouterConfiguration } from 'aurelia-router';

export class App {
    router: Router;

    configureRouter(config: RouterConfiguration, router: Router) {
        config.title = 'Aurelia';
        config.map([{
            route: [ '', 'home' ],
            name: 'home',
            settings: { icon: 'home' },
            moduleId: PLATFORM.moduleName('../home/home'),
            nav: true,
            title: 'Home'
        },{
            route: 'devices',
            name: 'devices',
            settings: { icon: 'star' },
            moduleId: PLATFORM.moduleName('../devices/devices'),
            nav: true,
            title: 'List of Devices'
        }, {
            route: 'firmwareinfo',
            name: 'firmwareinfo',
            settings: { icon: 'th' },
            moduleId: PLATFORM.moduleName('../firmwareinfo/firmwareinfo'),
            nav: true,
            title: 'List of Firmware'
        }, {
            route: 'backupinfo',
            name: 'backupinfo',
            settings: { icon: 'inbox' },
            moduleId: PLATFORM.moduleName('../backupinfo/backupinfo'),
            nav: true,
            title: 'List of Backup'
        }, {
            route: 'updatehistory',
            name: 'updatehistory',
            settings: { icon: 'list-alt' },
            moduleId: PLATFORM.moduleName('../updatehistory/updatehistory'),
            nav: true,
            title: 'List of History'
        }, {
            route: 'updateque',
            name: 'updateque',
            settings: { icon: 'road' },
            moduleId: PLATFORM.moduleName('../updateque/updateque'),
            nav: true,
            title: 'List of Que'
        }]);

        this.router = router;
    }
}
