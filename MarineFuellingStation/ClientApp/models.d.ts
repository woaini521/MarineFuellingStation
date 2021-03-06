﻿declare namespace server {
    export class resultJSON<T>{
        code: number;
        msg: string;
        data: T
    }
}

declare namespace ydui {
    export class actionSheetItem {
        label: string;
        callback: Function;
        stay?: boolean;
    }
}

declare namespace helper {
    export class filterBtn {
        id?: number;
        name: string;
        value?: any;
        actived: boolean;
    }
}

declare namespace excel {
    const enum dataType {
        未指定,
        客户,
        销售单,
        油仓,
        船舶清污,
        化验单,
        油仓测量,
        出入仓记录,
        转仓单
    }
}

declare namespace work {
    export interface JsSdkUiPackage {
        appId: string;
        timestamp: string;
        nonceStr: string;
        signature: string;
    }
    export interface JsSdkPayUiPackage extends JsSdkUiPackage {
        package: string;
    }

    export interface department {
        id: number;
        name: string;
        parentid: number;
        order: number;
    }

    export interface departmentListResult {
        errcode: number;
        errmsg: string;
        department: department[];
    }

    export interface attr {
        name: string;
        value: string;
    }

    export interface extattr {
        attrs: attr[];
    }

    export interface userlist {
        userid: string;
        name: string;
        department: number[];
        order: number[];
        position: string;
        mobile: string;
        gender: string;
        email: string;
        isleader: number;
        avatar: string;
        telephone: string;
        english_name: string;
        status: number;
        extattr: extattr;
    }

    export interface departmentMemberInfoResult {
        errcode: number;
        errmsg: string;
        userlist: userlist[];
    }

    export interface tagMemberResult {
        userlist: userlist[];
        partylist: any[];
        errcode: number;
        errorCodeValue: number;
        errmsg: string;
        p2PData?: any;
    }

    export interface memberResult {
        userid: string;
        name: string;
        department: number[];
        order: number[];
        position: string;
        mobile: string;
        gender: number;
        email: string;
        isleader: number;
        avatar: string;
        status: number;
        telephone: string;
        english_name: string;
        extattr: {
            attrs: any[];
        };
        enable: number;
        wxplugin_status: string;
    }
    export interface checkinData {
        userid: string;
        groupname: string;
        checkin_type: string;
        exception_type: string;
        checkin_time: Date;
        location_title: string;
        wifiname: string;
        notes: string;
        wifimac: string;
        mediaids: string[];
    }
    export interface checkinDataResult {
        errcode: number;
        errmsg: string;
        checkindata: checkinData[];
    }
}