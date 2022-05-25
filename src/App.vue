<template>
  <div v-show="viewAddProject" name="addProjectElement" class="addProjectTitle">Add new project</div>
  <AddElement v-show="viewAddProject" @addElement="addProject" textButton="ADD PROJECT" :addedObject= ProjectObj />
  <Projects :elements="projects" @deleteProject="deleteProject" @activeProject="activeProject" @editText="editText" />
  <div class="viewAddProjectBtn" @click="viewAddProjectEvent">NEW PROJECT</div>
</template>

<script>
import AddElement from './components/AddElement.vue'
import Projects from './components/Projects.vue'
export default {
  components: {
    AddElement,
    Projects
  },
  data () {
    return {
      projects: this.loadProjectsFromLS(),
      ProjectObj: {
        id: Number, text: String, active: Boolean
      },
      viewAddProject: false
    }
  },
  methods: {
    addProject (el) {
      var request = new XMLHttpRequest()
      console.log(el.text)
      const data = JSON.stringify({
        text: el.text
      })

      request.onreadystatechange = () => {
        if (request.readyState === 4 && request.status === 200) {
          el.id = parseInt(request.responseText)
          this.projects = [...this.projects, el]
          this.viewAddProject = true
        }
      }

      request.open('POST', 'https://localhost:5001/api/projectAdd', true)
      request.setRequestHeader('content-type', 'application/json')
      request.send(data)
    },
    deleteProject (id) {
      let val = -1
      this.projects.every((e, i) => {
        if (e.id === id) {
          this.projects.filter((el) => el.id !== id)
          val = 1
          return false
        }
        return true
      })

      if (val === -1) return

      const xhr = new XMLHttpRequest()
      xhr.open('GET', 'https://localhost:5001/api/projectDelete/' + id)
      xhr.onload = () => {
        if (xhr.status !== 200) alert('Ошибка: ' + xhr.status)
        else this.projects = this.projects.filter((el) => el.id !== id)
      }
      xhr.send()
    },
    activeProject (id) {
      let val = -1
      this.projects.every((e, i) => {
        if (e.id === id) {
          e.active = !e.active
          val = e.active
          return false
        }
        return true
      })

      if (val === -1) return

      const xhr = new XMLHttpRequest()
      xhr.open('GET', 'https://localhost:5001/api/projectActive/' + id + '/' + val)
      xhr.send()
    },
    check (id) {
      this.projects.forEach(e => {
        console.log(e.active)
      })
    },
    editText (id, text) {
      let val = -1
      let findElemId
      this.projects.every((e, i) => {
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
          this.projects[findElemId].text = text
        }
      }

      request.open('POST', 'https://localhost:5001/api/projectEditText', true)
      request.setRequestHeader('content-type', 'application/json')
      request.send(data)
    },
    saveProjectsToLS () {
      localStorage.setItem('projects', JSON.stringify(this.projects))
    },
    loadProjectsFromLS () {
      const xhr = new XMLHttpRequest()

      xhr.open('GET', 'https://localhost:5001/api/GetProjects')
      xhr.onload = () => {
        if (xhr.status !== 200) {
          return
        }
        console.log(typeof JSON.parse(xhr.response))
        console.log(JSON.parse(xhr.response))
        this.projects = JSON.parse(xhr.response)
      }
      xhr.send()
    },
    viewAddProjectEvent () {
      this.viewAddProject = true
      window.scrollTo(0, 0)
    }
  }
}
</script>

<style lang="scss">
#app {
  margin: 0 auto;
  width: 100%;
}
.addProjectTitle{
  font-size: 20pt;
  margin-bottom: .5%;
}
.viewAddProjectBtn{
  position: fixed;
  bottom: 0;
  right: 0;
  padding: 1rem;
  margin: 2.5rem;
  background-color: black;
  border-radius: 10px;
  cursor: pointer;
  font-size: 13pt;
}
</style>
