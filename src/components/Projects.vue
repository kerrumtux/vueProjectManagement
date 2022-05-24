<template>
  <div id="main">
    <div class="title">▼  Projects</div>
    <div v-for="elem in elements" :key="elem.id" >
      <Project :elem="elem" @deleteProject="$emit('deleteProject', elem.id)" @activeProject="$emit('activeProject', elem.id)" @editText="editTextEvent" />
    </div>
  </div>
</template>

<script>
import Project from './Project.vue'
export default {
  name: 'Projects',
  props: {
    elements: Array
  },
  emits: ['deleteProject', 'activeProject', 'editText'],
  components: {
    Project
  },
  methods: {
    editTextEvent (id, value) {
      this.$emit('editText', id, value)
    }
  }
}
</script>

<style lang="scss" scoped>
.title {
  font-size: 20pt;
  margin: 0% 0 1% 0
}
</style>
