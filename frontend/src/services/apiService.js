import axios from 'axios';

export const baseURL = 'http://localhost:43056';

function build(form) {
  return {
    PermanentCode: form.permanentCode,
    University: form.university,
    GraduationYear: form.graduationYear,
    Program: form.program,
    Gpa: form.gpa,
  };
}

export const getUniversities = () => axios({
  method: 'get',
  baseURL,
  url: 'api/universities',
});

export const getPrograms = (universityName, degree, year) => axios({
  method: 'get',
  baseURL,
  url: 'api/programs',
  params: { universityName, degree, year, },
});

export const getProgramSpecs = () => axios({
  method: 'get',
  baseURL,
  url: 'api/programs/specs',
});

export const verifyCertificate = form => axios({
  method: 'post',
  baseURL,
  url: 'api/files',
  data: build(form),
  config: { headers: { 'Content-Type': 'application/json' } },
});
