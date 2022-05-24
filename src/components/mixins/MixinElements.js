export default {
  name: 'MixinElements',
  props: {
    elements: Array
  },
  emits: ['deleteProject', 'activeProject', 'editText'],
  methods: {
    editTextEvent (id, value) {
      this.$emit('editText', id, value)
    }
  }
}
