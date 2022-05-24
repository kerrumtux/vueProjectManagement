<template>
    <div class="project" style="background-color:black">
      <div class="main">
        <button v-show="!viewTasks && !addTask" class="leftButton" @click="viewTasksEvent">▼</button>
        <button v-show="viewTasks && !addTask" class="leftButton" @click="viewTasksEvent">▲</button>
        <button v-show="viewTasks && addTask" class="leftButton" @click="closeSubElements">▲</button>
        <span v-show="!edit" :style="cssVars">{{ value }}</span>
        <input v-show="edit" :style="cssVars" v-model="value" />
        <button v-show="edit && !viewTasks" :style="cssVars" style="background-color: red;" class="rightButton deleteProject" @click="$emit('deleteProject', elem.id)">
            DELETE
        </button>
        <button v-show="!edit && elem.active" :style="cssVars" style="background-color: green;" class="rightButton editProject" @click="$emit('activeProject')">
            ACTIVE
        </button>
        <button v-show="!edit && !elem.active" :style="cssVars" style="background-color: red;" class="rightButton editProject" @click="$emit('activeProject')">
            DISACTIVE
        </button>
        <button v-show="edit" :style="cssVars" class="rightButton" style="background-color: green;" @click="close">
          OK & CLOSE
        </button>
        <button class="rightButton" v-show="!edit && viewTasks && !addTask" :style="cssVars" @click="addTaskEvent">
            ADD TASK
        </button>
        <button class="rightButton" v-show="viewTasks && addTask" :style="cssVars" @click="closeTaskEvent">
            CLOSE TASK
        </button>
        <button class="rightButton" v-show="!edit && !viewTasks" :style="cssVars" @click="view">
            EDIT
        </button>
      </div>
      <AddElement v-if="addTask"  textButton="Add" :projectId="elem.id" :addedObject=TaskObj @addElement="addTaskE" />
      <div v-if="viewTasks && tasks.length > 0">
        <p style='font-size: 14pt; margin: 2% 0 2% 0'>▼  Tasks</p>
        <Tasks :elements=tasks @deleteTask="deleteTask" @activeTask="activeTask" @editText="editText" />
      </div>
    </div>
</template>

<script>
import Value from './mixins/Value.js'
import AddElement from './AddElement.vue'
import Tasks from './Tasks.vue'
console.log(Value)

export default {
  components: {
    AddElement,
    Tasks
  },
  name: 'Project',
  mixins: [Value],
  data () {
    return {
      tasks: [],
      viewTasks: false,
      addTask: false,
      TaskObj: {
        id: Number,
        text: String,
        active: Boolean,
        projectId: Number
      }
    }
  },
  props: {
    elem: Object
  },
  methods: {
    addTaskE (el) {
      var request = new XMLHttpRequest()
      console.log(el.text)
      const data = JSON.stringify({
        text: el.text,
        projectId: this.elem.id
      })

      request.onreadystatechange = () => {
        if (request.readyState === 4 && request.status === 200) {
          el.id = parseInt(request.responseText)
          this.tasks = [...this.tasks, el]
          this.addTask = false
        }
      }

      request.open('POST', 'https://localhost:5001/api/taskAdd')
      request.setRequestHeader('content-type', 'application/json')
      request.send(data)
    },
    saveTask () {
      localStorage.setItem('tasks' + this.elem.id, JSON.stringify(this.tasks))
    },
    loadTasks () {
      const xhr = new XMLHttpRequest()
      xhr.open('GET', 'https://localhost:5001/api/GetTasks/' + this.elem.id)
      xhr.onload = () => {
        if (xhr.status !== 200) {
          return
        }
        console.log(typeof JSON.parse(xhr.response))
        console.log(JSON.parse(xhr.response))
        this.tasks = JSON.parse(xhr.response)
      }
      xhr.send()
      console.log(xhr)
    },
    deleteTask (id) {
      let val = -1
      this.tasks.every((e, i) => {
        if (e.id === id) {
          val = 1
          return false
        }
        return true
      })

      if (val === -1) return

      const xhr = new XMLHttpRequest()
      xhr.open('GET', 'https://localhost:5001/api/taskDelete/' + id)
      xhr.onload = () => {
        if (xhr.status !== 200) alert('Ошибка: ' + xhr.status)
        else this.tasks = this.tasks.filter((el) => el.id !== id)
      }
      xhr.send()
    },
    activeTask (id) {
      let val = -1
      let findElement
      this.tasks.every((e, i) => {
        if (e.id === id) {
          val = !e.active
          findElement = e
          return false
        }
        return true
      })

      if (val === -1) return

      const xhr = new XMLHttpRequest()
      xhr.open('GET', 'https://localhost:5001/api/taskActive/' + id + '/' + val)
      xhr.onload = () => {
        if (xhr.status !== 200) alert('Ошибка: ' + xhr.status)
        else findElement.active = !findElement.active
      }
      xhr.send()
    },
    editText (id, text) {
      let val = -1
      let findElemId
      this.tasks.every((e, i) => {
        if (e.id === id) {
          findElemId = i
          val = 1
          return false
        }
        return true
      })

      if (val === -1) return

      var request = new XMLHttpRequest()
      const data = JSON.stringify({
        id: id,
        text: text
      })

      request.onreadystatechange = () => {
        if (request.readyState === 4 && request.status === 200) {
          this.tasks[findElemId].text = text
        }
      }

      request.open('POST', 'https://localhost:5001/api/taskEditText', true)
      request.setRequestHeader('content-type', 'application/json')
      request.send(data)
    },
    viewTasksEvent () {
      this.loadTasks()
      this.viewTasks = !this.viewTasks
    },
    closeSubElements () {
      this.addTask = false
      this.viewTasks = false
    },
    addTaskEvent () {
      this.addTask = true
    },
    closeTaskEvent () {
      this.addTask = false
    },
    close () {
      console.log('editText', this.elem.id, this.value)
      this.$emit('editText', this.elem.id, this.value)
      this.edit = false
    }
  }
}
</script>

<style lang="scss" scoped>
  .project{
    margin-bottom: 2%;
  }
  .main{
    background-color: rgb(0,0,0);
    height: 5rem;
  }
  .main div{
    height: 50rem;
  }
  input{
    height: 100%;
  }
  span, input{
    width: 75%;
    display: inline-block;
    padding: 0 0 0 1%;
    margin: 0;
  }
  button{
    display: inline-block;
    height: 5rem;
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
</style>
