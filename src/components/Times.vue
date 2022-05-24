<template>
  <div v-for="elem in elements" :key="elem.id" >
    <div class="main">
        <span>{{ elem.text }}</span>
        <span>{{ elem.numberOfHours }}</span>
        <span>{{ elem.date }}</span>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Times',
  props: {
    elements: Array
  }
}
</script>

<style lang="scss" scoped>

  .main{
    height: 6rem;
    position: relative;
    border-bottom: 3px solid black;
  }
  span{
    font-size: 14pt;
    display: inline-block;
    margin: 0;
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    text-align: end;
  }
  span:nth-child(2){
    left: calc(90%/3);
  }
  span:nth-child(3){
    left: calc(90% - 90%/3);
  }
</style>
