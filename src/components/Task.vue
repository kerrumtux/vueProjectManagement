<template>
    <div class="project">
      <div class="main">
        <button v-show="!edit && elem.active" :style="cssVars" class="editProject" style="background-color: green;" @click="$emit('activeTask')">
          ACTIVE
        </button>
        <button v-show="!edit && !elem.active" :style="cssVars" class="editProject" style="background-color: red;" @click="$emit('activeTask')">
          DISACTIVE
        </button>
        <button v-show="edit" :style="cssVars" style="background-color: green;" @click="close">
          OK & CLOSE
        </button>
        <div id="nameContainer" v-show="!edit" :style="cssVars">
          <p>{{ value }}</p>
        </div>
        <input v-show="edit" :style="cssVars" v-model="value" />
        <button v-show="edit" :style="cssVars" class="deleteTask" style="background-color: red;" @click="$emit('deleteTask', elem.id)">
          DELETE
        </button>
        <button class="btnViewTime" @click="viewTimeE">TIME MNG.</button>
        <button v-show="!edit" :style="cssVars" @click="view">
          EDIT
        </button>
      </div>
      <div class="blackoutLayer" v-show="timeView"></div>

      <div v-show="timeView" id="popupWindow">
        <div class="selectViewByDay">
          <select @change="selectViewer">
            <option value="all">All</option>
            <option value="specific">Specific</option>
            <option value="lastMonth">Last month</option>
          </select>
        </div>
        <div v-if="viewType===-1" class="selectViewByDay"><input @change="viewTimeByDay" type="date"/></div>
        <!-- <div class="closeButton" @click="viewTimeE"><span>&#10006;</span></div> -->
        <div class="closeButton" @click="viewTimeE"></div>
        <AddUsedTime @addElement="addTimeUsed" />
        <Times :elements=timesUsedSpecific />
      </div>
    </div>
</template>

<script>
import Value from './mixins/Value.js'
import AddUsedTime from './AddUsedTime.vue'
import Times from './Times.vue'
export default {
  name: 'Task',
  mixins: [Value],
  components: { AddUsedTime, Times },
  data () {
    return {
      timesUsed: [],
      timesUsedSpecific: [],
      timeView: false,
      viewType: 0 // 0 - all, -1 - per day, 1 - per month
    }
  },
  methods: {
    addTimeUsed (el) {
      const data = {
        text: el.text.trim(),
        taskId: this.elem.id,
        date: el.date,
        numberOfHours: parseInt(el.numberOfHours)
      }

      const validate1 = typeof data.text !== 'string' || data.text === '' || typeof data.taskId !== 'number'
      const validate2 = !/^\d{4}(-\d{2}){2}$/.test(data.date) || isNaN(data.numberOfHours || data.numberOfHours === 0)
      if (validate1 || validate2) {
        alert('Некорректно заполнены поля ввода')
        return
      }

      let summ = data.numberOfHours

      this.timesUsed.forEach(e => {
        if (e.date === el.date) summ += e.numberOfHours
      })

      if (summ > 24) {
        alert('Количество часов за эту дату превышает 24. Проводка не может быть добавлена')
        return
      }

      const request = new XMLHttpRequest()
      request.onreadystatechange = () => {
        if (request.readyState === 4 && request.status === 200) {
          this.timesUsed = [...this.timesUsed, el]
          this.timesUsedSpecific = this.timesUsed
        }
      }

      request.open('POST', 'https://localhost:5001/api/timeAdd')
      request.setRequestHeader('content-type', 'application/json')
      request.send(JSON.stringify(data))
    },
    saveTime () {
      localStorage.setItem('times' + this.elem.id, JSON.stringify(this.timesUsed))
    },
    loadTimes () {
      const xhr = new XMLHttpRequest()
      xhr.open('GET', 'https://localhost:5001/api/GetTimes/' + this.elem.id)
      xhr.onload = () => {
        if (xhr.status !== 200) {
          return
        }
        const times = JSON.parse(xhr.response)
        this.timesUsed = times
        this.timesUsedSpecific = times
      }
      xhr.send()
    },
    viewTimeE () {
      this.loadTimes()
      this.timeView = !this.timeView
    },
    selectViewer (e) {
      switch (e.target.value) {
        case 'all' :
          this.timesUsedSpecific = this.timesUsed
          this.viewType = 0
          break
        case 'specific' :
          this.viewType = -1
          break
        case 'lastMonth' :
          this.viewType = 1
          this.viewTimeByLastMonth()
          break
      }
    },
    viewTimeByDay (e) {
      this.timesUsedSpecific = this.getTimeByDay(e.target.value)
    },
    viewTimeByLastMonth (e) {
      this.timesUsedSpecific = this.getTimeByLastMonth()
    },
    getTimeByDay (dayStr) {
      const arr = []
      this.timesUsed.forEach(e => {
        if (dayStr === e.date) arr.push(e)
      })
      return arr
    },
    getTimeByLastMonth () {
      const arr = []
      let date = new Date().getMonth() + 1
      if (date < 10) date = '0' + date
      else date = date.toString()
      this.timesUsed.forEach(e => {
        if (date === e.date.substring(5, 7)) arr.push(e)
      })
      return arr
    },
    close () {
      this.$emit('editText', this.elem.id, this.value)
      this.edit = false
    }
  }
}
</script>

<style lang="scss" scoped>
  .main{
    background-color: black;
    height: 4rem;
    margin-bottom: .3%;
  }
  #nameContainer, .main > input{
    width: 80%;
    display: inline-block;
    padding: 0 0 0 1%;
    margin: 0;
  }
  .main > input{
    height: 100%;
  }
  #nameContainer{
    height: 100%;
    border-left: 4px solid white;
  }
  p{
    position: relative;
    top: 50%;
    transform: translateY(-50%);
  }
  #popupWindow .selectViewByDay {
    width: 15%;
    font-size: 14px;
    height: 4rem;
    color: 'var(--dark)';
    background-color: white;
    color: black;
    display: inline-block;
    overflow: hidden;
    border-right: 1px solid black;

    input, select{
      width: 100%;
      height: 100%;
    }
  }
  .main button{
    display: inline-block;
    height: 100%;
    width: calc(20%/3);
    float: right;
  }
  .rightButton{
    width: var(--button-width);
  }
  .leftButton{
    width: 5%;
  }
  .project-title, .project-actions{
      display: inline-block;
  }
  #popupWindow{
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 90%;
    height: 85%;
    background-color: var(--bg-color);
    z-index: 2;
    overflow: scroll;
    scrollbar-width: none;
  }
  select{
    border-radius:0px !important;
    border: 0;
    border-right: 1px solid black;
  }
  .closeButton{
    position: absolute;
    right: 0;
    top: 0;
    width: 2rem;
    height: 2rem;
    margin: 1rem 1rem 0 0;
    cursor: pointer;
    background: url("../../public/icon/close.png") 0 0/auto 100% no-repeat;

    span{
      position: absolute;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      font-size: 2rem;
    }
  }

  .blackoutLayer{
    background-color: black;
    opacity: 0.5;
    width: 100%;
    height: 100%;
    position: fixed;
    top: 0;
    left: 0;
    z-index: 1;
  }
</style>
