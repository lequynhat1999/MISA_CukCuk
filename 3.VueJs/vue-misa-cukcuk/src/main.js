import Vue from 'vue'
import App from './App.vue';
import axios from 'axios';
import VueAxios from 'vue-axios';
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";
import VueTheMask from 'vue-the-mask';
import SlideUpDown from 'vue-slide-up-down';
import money from 'v-money';
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';
const options = {
  // You can set your default options here
  position: 'bottom-right'
};


import { ValidationProvider, ValidationObserver, extend } from 'vee-validate'; 
import * as rules from 'vee-validate/dist/rules';
import { messages } from 'vee-validate/dist/locale/vi.json';

Object.keys(rules).forEach(rule => {
  extend(rule, {
    ...rules[rule], // copies rule configuration
    message: messages[rule] // assign message
  });
});

// Register it globally
Vue.component('ValidationProvider', ValidationProvider); // validate field
Vue.component('ValidationObserver', ValidationObserver); // validate form

Vue.use(Toast, options);
Vue.component('datepicker', DatePicker)
Vue.config.productionTip = false
Vue.use(VueAxios, axios,VueTheMask)
Vue.use(money)
Vue.component('slide-up-down', SlideUpDown)
new Vue({
  render: h => h(App),
}).$mount('#app')
