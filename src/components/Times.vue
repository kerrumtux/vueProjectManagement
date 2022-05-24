<template>
  <div v-for="elem in elements" :key="elem.id" >
    <div class="main">
        <p>{{ elem.text }}</p>
        <p>{{ elem.numberOfHours }}</p>
        <p>{{ elem.date }}</p>
    </div>
  </div>
</template>

<script>
import MixinElements from './mixins/MixinElements'
export default {
  name: 'Times',
  props: {
    elements: Array
  },
  mixins: [MixinElements]
}
</script>

<style lang="scss" scoped>

  .main{
    height: 6rem;
    position: relative;
    border-bottom: 1px solid black;
    background-color: var(--bg-color);
  }
  p{
    font-size: 14pt;
    width: calc(90%/3);
    display: inline-block;
    margin: 0;
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    text-align: end;
  }
  p:nth-child(2){
    left: calc(90%/3);
  }
  p:nth-child(3){
    left: calc(90% - 90%/3);
  }
</style>
