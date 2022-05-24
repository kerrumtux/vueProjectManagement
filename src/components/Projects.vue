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
import MixinElements from './mixins/MixinElements'
export default {
  name: 'Projects',
  mixins: [MixinElements],
  components: {
    Project
  }
}
</script>

<style lang="scss" scoped>
.title {
  font-size: 20pt;
  margin: 0% 0 1% 0
}
</style>
