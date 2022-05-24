<template>
  <div id="main" v-for="elem in elements" :key="elem.id" >
    <Task :elem="elem" @deleteTask="$emit('deleteTask', elem.id)" @activeTask="$emit('activeTask', elem.id)" @editText="editTextEvent" />
  </div>
</template>

<script>
import Task from './Task.vue'
export default {
  name: 'Tasks',
  props: {
    elements: Array
  },
  components: {
    Task
  },
  emits: ['deleteTask', 'activeTask', 'editText'],
  methods: {
    editTextEvent (id, value) {
      this.$emit('editText', id, value)
    }
  }
}
</script>

<style lang="scss" scoped>
#main {
  margin-bottom: .1%;
}
</style>
