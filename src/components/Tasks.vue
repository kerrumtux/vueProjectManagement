<template>
  <div id="main" v-for="elem in elements" :key="elem.id" >
    <Task :elem="elem" @deleteTask="$emit('deleteTask', elem.id)" @activeTask="$emit('activeTask', elem.id)" @editText="editTextEvent" />
  </div>
</template>

<script>
import Task from './Task.vue'
import MixinElements from './mixins/MixinElements'
export default {
  name: 'Tasks',
  props: {
    tasks: Array
  },
  components: {
    Task
  },
  mixins: [MixinElements],
  emits: ['deleteTask', 'activeTask']
}
</script>

<style lang="scss" scoped>
#main {
  margin-bottom: .1%;
}
</style>
