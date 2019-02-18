<template>
  <div>
    <md-toolbar id="toolbar">
      <div class="md-toolbar-section-start">
        <img id="logo" src="../assets/mantle-logo.png">
        <h3 class="md-title">{{title}}</h3>
      </div>
    </md-toolbar>
  </div>
</template>

<script>
export default {
  name: "headerbar",
  data() {
    return {
      title: "Credentials Verification",
      inline: null
    };
  }
};
</script>

<style lang="scss" scoped>
.md-focused label {
  font-weight: 500;
}
#toolbar {
  position: fixed;
  z-index: 10;
}
#searchbar {
  width: 200px;
}
#logo {
  height: 30px;
}
</style>
