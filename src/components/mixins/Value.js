export default {
  props: {
    elem: Object
  },
  data () {
    return {
      edit: false,
      value: this.elem.text
    }
  },
  methods: {
    view (event) {
      this.edit = true
    }
  },
  computed: {
    cssVars () {
      return {
        // '--button-width': this.edit ? ((40 / 3).toString() + '%') : '20%',
        '--button-width': '10%',
        '--p-width': this.edit ? '60%' : '60%'
      }
    }
  }
}
