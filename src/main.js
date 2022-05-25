import { createApp } from 'vue'
import App from './App.vue'
import './static/main.css'

createApp(App).mount('#app')

window.objectValidation = function (object, cookieArr) {
  if (object === {}) return false

  let error = false
  const cookieKeys = Object.keys(object)

  cookieArr.every(function (element, index) {
    if (cookieKeys.includes(element[0])) {
      const stringComp = typeof object[element[0]]
      if (element[1] !== 'json' && stringComp === element[1].toString()) {
        console.log('')
      } else if (typeof object[element[0]] === 'string') {
        try {
          JSON.parse(object[element[0]])
        } catch (e) {
          error = true
          // console.log('eeeer')
        }
      } else error = true
    // return false;
    }

    if (error === true) {
      console.log('elem error ', element)
      return false
    }
  })

  return !error
}
