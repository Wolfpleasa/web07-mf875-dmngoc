import Vue from 'vue'
import App from './App.vue'

import VueRouter from "vue-router";
import axios from 'axios'
import VueAxios from 'vue-axios'
import SlideUpDown from 'vue-slide-up-down';
import EmployeeList from "./view/employee/EmployeeList.vue";
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';


Vue.use(VueRouter);
Vue.use(VueAxios, axios)
Vue.config.productionTip = false
Vue.component('slide-up-down', SlideUpDown)
Vue.component('datepicker', DatePicker);

const routes = [
    { path: '/employee', name: 'Employee', component: EmployeeList },
    { path: '', redirect: '/employee' },
]

const router = new VueRouter({
    mode: 'history',
    routes,
})

new Vue({
    router,
    render: h => h(App),
}).$mount('#app')