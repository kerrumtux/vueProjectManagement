<template>
  <section>
      <div>
        <div class="inputContainer">
          <input v-model="val1" type="date"/>
        </div>
        <div class="inputContainer">
          <input v-model="val2" type="text" placeholder="Number of hours:"/>
        </div>
        <div class="inputContainer">
          <input v-model="val3" type="text" placeholder="Description:"/>
        </div>
        <div class="addButtonContainer" @click="make"><span>ADD</span></div>
      </div>
  </section>
</template>

<script>
export default {
  name: 'AddUsedTime',
  components: {
  },
  props: {
    addedObject: Object
  },
  data () {
    return {
      text: '',
      active: true,
      value: '',
      val1: '',
      val2: '',
      val3: ''
    }
  },
  methods: {
    make () {
      if (this.val1.trim() === '' || this.val3.trim() === '') {
        alert('Заполните все необходимые поля')
        return
      }

      const num = parseInt(this.val2)
      if (Number.isInteger(num) !== true || num > 24 || num < 1) {
        alert('Введите число часов от 1 до 24')
        return
      }

      const obj = {}

      obj.timeId = Math.random()
      obj.date = this.val1
      obj.numberOfHours = parseInt(this.val2)
      obj.text = this.val3
      console.log('ok')
      this.$emit('addElement', obj)
      this.val1 = ''
      this.val2 = ''
      this.val3 = ''
    }
  }
}
</script>

<style lang="scss" scoped>
  section {
    margin-bottom: 1%;
  }

  .inputContainer{
    width: 30%;
    font-size: 14px;
    height: 5rem;
    color: 'var(--dark)';
    background: 'var(--text-color)';
    display: inline-block;
    overflow: hidden;
    border-right: 1px solid black;
  }

  input{
    width: 100%;
    height: 100%;
    padding: 1rem;
  }

  .addButtonContainer{
    width: 10%;
    font-size: 14px;
    height: 5rem;
    color: 'var(--dark)';
    background: 'var(--text-color)';
    display: inline-block;
    overflow: hidden;
    background-color: white;
    color: black;
    position: relative;
    cursor: pointer;

    span{
      position: absolute;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
    }
  }

  button{
    width: 100%;
    height: 100%;
  }
</style>
