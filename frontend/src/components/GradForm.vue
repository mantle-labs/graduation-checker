<template>
  <div id="gradform">
    <div class="loader" v-if="loadingData">
         <md-progress-spinner md-mode="indeterminate">
         </md-progress-spinner>
    </div>
    <form id="gradformlayout" class="md-layout" novalidate>
      <md-card class="md-layout-item">
        <md-card-header>
          <div class="md-title">Graduation Information</div>
        </md-card-header>

        <md-card-content>
          <div class="md-layout-item md-small-size-100">
            <md-field>
              <label for="permanent-code">Ontario Education Number</label>
              <md-input name="permanent-code" id="permanent-code" v-model="form.permanentCode" :disabled="sending" />
            </md-field>
          </div>

          <div class="md-layout-item md-small-size-100">
            <md-field>
              <label for="university">University</label>
              <md-select name="university" id="university" v-model="form.university" v-on:md-selected="getPrograms" md-dense :disabled="sending">
                <md-option v-for="(university, index) in universities" :key="index" :value="university.id">{{university.longName}}</md-option>
              </md-select>
            </md-field>
          </div>

          <div class="md-layout-item md-small-size-100">
            <md-field>
              <label for="degree">Degree</label>
              <md-select name="degree" id="degree" v-model="form.degree" v-on:md-selected="getPrograms" md-dense :disabled="sending">
                <md-option v-for="(degree, index) in degrees" :key="index" :value="degree">{{degree}}</md-option>
              </md-select>
            </md-field>
          </div>

          <div class="md-layout-item md-small-size-100">
            <md-field>
              <label for="graduation-year">Graduation Year</label>
              <md-select name="graduation-year" id="graduation-year" v-model="form.graduationYear" v-on:md-selected="getPrograms" md-dense :disabled="sending">
                <md-option v-for="(year, index) in years" :key="index" :value="new Date().getFullYear() - index">{{new Date().getFullYear() - index}}</md-option>
              </md-select>
            </md-field>
          </div>

          <div class="md-layout-item md-small-size-100">
            <md-field>
              <label for="program">Program</label>
              <md-select name="program" id="program" v-model="form.program" md-dense :disabled="sending || !isReadyToInputProgram()">
                <md-option v-for="(program, index) in programs" :key="index" :value="program.id">{{program.longName}}</md-option>
              </md-select>
            </md-field>
          </div>

          <div class="md-layout-item md-small-size-100">
            <md-field>
              <label for="gpa">Gpa (#.##)</label>
              <md-input type="number" id="gpa" name="gpa" v-model="form.gpa" :disabled="sending" />
            </md-field>
          </div>
        </md-card-content>

        <md-progress-bar md-mode="indeterminate" v-if="sending" />

        <md-card-actions>
          <md-button class="md-primary" v-on:click="sendForm" :disabled="sending">Verify Graduation</md-button>
        </md-card-actions>
      </md-card>

      <md-snackbar :md-active.sync="showResponse" v-html="response"></md-snackbar>
    </form>
  </div>
</template>

<script>
import * as apiService from "../services/apiService";

export default {
  name: "GradForm",
  data: () => ({
    form: {
      permanentCode: null,
      university: null,
      degree: null,
      graduationYear: null,
      program: null,
      gpa: null
    },
    universities: [],
    universitiesById: {},
    degrees: [],
    years: [],
    programs: [],
    sending: false,
    showResponse: false,
    response: null,
    loadingData: false,
  }),
  methods: {
    isFilledForm() {
      return (
        this.form.permanentCode &&
        this.form.university &&
        this.form.degree &&
        this.form.graduationYear &&
        this.form.program &&
        this.form.gpa
      );
    },
    isReadyToInputProgram() {
      return (
        this.form.university && this.form.degree && this.form.graduationYear
      );
    },
    sendForm() {
      if (!this.isFilledForm()) {
        this.setResponse("Please fill in all fields.");
        return;
      }

      this.sending = true;
      apiService
        .verifyCertificate(this.form)
        .then(body => {
          if (body.data.isValid) {
            this.setResponse("The information provided is exact.");
          } else {
            this.setResponse(
              `The following information is incorrect. See the differences <a href="${body.data.fileDiff}" target="_blank">here</a>.`
            );
          }
          this.sending = false;
        })
        .catch(err => {
          this.setResponse("No profile was found for that person.");
          this.sending = false;
        });
    },
    getUniversities() {
      this.loadingData = true;

      apiService.getProgramSpecs()
        .then(body => {
          this.degrees = body.data.PD || [];
          this.years = body.data.PY || [];
        });

      apiService.getUniversities()
        .then(body => {
          this.universities = body.data;
          this.universities.forEach(uni => {
            this.universitiesById[uni.id] = uni;
          });
          this.loadingData = false;
        });
    },
    getPrograms() {
      if (
        !this.form.university ||
        !this.form.degree ||
        !this.form.graduationYear
      ) {
        return;
      }

      this.form.program = null;
      this.loadingData = true;
      apiService
        .getPrograms(
          this.universitiesById[this.form.university].shortName,
          this.form.degree,
          this.form.graduationYear
        ).then(body => {
          this.programs = body.data;
          this.loadingData = false;
        });
    },
    setResponse(msg) {
      this.response = msg;
      this.showResponse = true;
    }
  },
  mounted() {
    this.getUniversities();
  }
};
</script>

<style lang="scss" scoped>
#gradform {
  padding-top: 100px;

  .loader {
    position: fixed;
    background-color: rgba(0,0,0,0.2);
    top: 0;
    bottom: 0;
    right: 0;
    left: 0;
    z-index: 100;

    .md-progress-spinner {
      top: calc(50% - 32px);
    }
  }
}
#gradformlayout {
  margin: auto;
  width: 500px;
}
.md-progress-bar {
  position: absolute;
  top: 0;
  right: 0;
  left: 0;
}
</style>