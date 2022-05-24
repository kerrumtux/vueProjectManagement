<template>
  <div>
    <input v-model="value" type="text" placeholder="Enter text:"/>
    <button type="submit" @click="submitProjectEvent">{{ textButton }}</button>
  </div>
</template>

<script>
export default {
  name: 'InputAndButton',
  data () {
    return {
      value: '',
      width: '200px',
      styleObj: {
        color: 'var(--dark)',
        background: 'var(--text-color)'
      }
    }
  },
  props: {
    textButton: String
  },
  methods: {
    submitProjectEvent () {
      console.log('val', this.value)
      this.$emit('submitProjectEvent', this.value)
      this.value = ''
    }
  },
  emits: ['submitProjectEvent']
}
</script>

<style lang="scss" scoped>
button{
  width: 20%;
  font-size: 14px;
  padding: 1rem;
  background-color: white;
  border-left: 2px solid black;
  color: black;
  height: 100%;
}
input{
  width: 80%;
  font-size: 14px;
  padding: 1rem;
  height: 100%;
  color: 'var(--dark)';
  background: 'var(--text-color)';
}
div{
  height: 5rem;
}
</style>
