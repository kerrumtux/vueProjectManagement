<template>
  <section>
    <form @submit="submitProject">
      <div>
        <InputAndButton :textButton="textButton" @submitProjectEvent="submitProjectEvent"/>
      </div>
    </form>
  </section>
</template>

<script>
import InputAndButton from './InputAndButton.vue'
export default {
  name: 'AddElement',
  components: {
    InputAndButton
  },
  props: {
    addedObject: Object,
    textButton: String
  },
  data () {
    return {
      text: '',
      active: true
    }
  },
  methods: {
    submitProject (e) {
      console.log('submit', this.text)
      e.preventDefault()
      if (!this.text) {
        alert('No text')
      }

      const newProject = {
        id: Math.random()
      }

      Object.keys(this.addedObject).forEach(element => {
        if (element === 'id') return
        newProject[element] = this[element]
      })

      Object.keys(newProject).forEach(e => console.log(e, newProject[e]))

      this.$emit('addElement', newProject)

      this.text = ''
      this.reminder = true
    },
    submitProjectEvent (str) {
      console.log('viewDetails', str)
      this.text = str
    }
  },
  emits: ['submitProjectEvent']
}
</script>

<style lang="scss" scoped>
  section {
    margin-bottom: 2%;
  }
</style>
