﻿import ComponentBase from "../../ComponentBase";
import { Component } from 'vue-property-decorator';
import moment from 'moment';
import axios from "axios";

@Component
export default class MyOrderComponent extends ComponentBase {

    show4: boolean = false;
    filterBtns: Array<helper.filterBtn>;
    activedBtnId: number;
    startDate: string;
    endDate: string;
    orders: server.order[];

    page: number;
    scrollRef: any;
    pSize: number = 30;

    timesubmit(): void {
        this.show4 = false;
    };

    constructor() {
        super();

        this.orders = new Array<server.order>();

        this.filterBtns = [
            { id: 0, name: '今日', actived: true },
            { id: 1, name: '昨日', actived: false },
            { id: 2, name: '本周', actived: false },
            { id: 3, name: '本月', actived: false }

        ];
        this.startDate = this.formatDate(new Date(), "YYYY-MM-DD");
        this.endDate = this.startDate;
        this.activedBtnId = 0;

        this.getOrders();
    }

    loadList() {
        this.getOrders((list: server.order[]) => {
            this.orders = this.page > 1 ? [...this.orders, ...list] : this.orders;
            this.scrollRef = (<any>this).$refs.infinitescroll;
            if (list.length < this.pSize) {
                this.scrollRef.$emit("ydui.infinitescroll.loadedDone");
                return;
            }

            //通知加载数据完毕
            this.scrollRef.$emit("ydui.infinitescroll.finishLoad");

            if (list.length > 0)
                this.page++;
            else
                this.page = 1;
            console.log("page = " + this.page)
        });
    }

    switchBtn(o: any, idx: number) {
        if (o.id != this.activedBtnId && this.activedBtnId != -1) {
            o.actived = true;
            this.filterBtns[this.activedBtnId].actived = false;
            this.activedBtnId = o.id;
        }
        if (this.activedBtnId == -1) {
            o.actived = true;
            this.activedBtnId = o.id;
        }   
        this.matchDate(o);
    }

    matchDate(o: any) {
        let today = this.formatDate(new Date(), "YYYY-MM-DD");
        switch (o.name) {
            case '今日':
                this.startDate = today;
                this.endDate = today;
                break;
            case '昨日':
                this.startDate = this.formatDate(moment(today).add(-1).toDate(), "YYYY-MM-DD");
                this.endDate = this.startDate;
                break;
            case '本周':
                this.startDate = this.formatDate(moment().weekday(1).toDate(), "YYYY-MM-DD");
                this.endDate = this.formatDate(moment().weekday(7).toDate(), "YYYY-MM-DD");
                break;
            case '本月':
                this.startDate = this.formatDate(moment().startOf('month').toDate(), "YYYY-MM-DD");
                this.endDate = this.formatDate(moment().endOf('month').toDate(), "YYYY-MM-DD");
                break;
        }
        this.refresh();
    }

    strCommission(commission: number, state: server.payState) {
        if (commission == 0) {
            if (state == server.payState.挂账) return "挂账";
            if (state == server.payState.未结算) return "未结算";
        }
        else if (commission < 0)
            return "业务费：￥" + commission
        else if (commission > 0)
            return "提成：￥" + commission
    }

    getTotalSalesComm() {
        let sum = 0;
        let sum1 = 0;
        this.orders.forEach((o, idx) => {
            sum += Math.round(o.salesCommission);
            if (o.payState == server.payState.挂账 || o.payState == server.payState.未结算)
                sum1 += Math.round(o.salesCommission);
        });
        return "应提：￥" + sum + " 未提：￥" + sum1;
    }

    godetail(id: number) {
        this.$router.push('/sales/order/' + id + '/myorder');
    }

    query() {
        if (this.activedBtnId != -1)
            this.filterBtns[this.activedBtnId].actived = false;
        this.activedBtnId = -1;
        this.refresh();
    }

    refresh() {
        this.page = 1;
        this.getOrders();
    }

    mounted() {
        this.$emit('setTitle', this.$store.state.username + ' 的销售单');
    }

    change(label: string, tabkey: string) {
        console.log(label);
    }

    getOrders(callback?: Function) {
        let sTimespan = ' 00:00';
        let eTimespan = ' 23:59';
        if (this.startDate == null) this.startDate = this.formatDate(new Date(), "YYYY-MM-DD");
        if (this.endDate == null) this.endDate = this.startDate;
        if (this.page == null) this.page = 1;
        axios.get('/api/Order/GetMyOrders?page=' + this.page.toString() + '&size=30&startDate=' + this.startDate + sTimespan + '&endDate=' + this.endDate + eTimespan)
            .then((res) => {
                let jobj = res.data as server.resultJSON<server.order[]>;
                if (jobj.code == 0) {
                    if (callback) {
                        callback(jobj.data);
                    }
                    else {
                        this.orders = jobj.data;
                        this.page++;
                    }
                }
            });
    }
}