import httpClient from '../servicios/HttpClient';

export const resgistrarUsuario = usuario=>{
    return new Promise((resolve,eject)=>{
        httpClient.post('/usuario/registrar',usuario).then(response=>{
            resolve(response);
        });
    })
};

export const actualizarUsuario = (usuario) => {
    return new Promise((resolve,eject)=>{
        httpClient.put('/usuario/',usuario).then(response=>{
            resolve(response);
        });
    })
};

export const obtenerUsuarioActual = () => {
  return new Promise((resolve, eject) => {
    httpClient.get("/usuario/").then((response) => {
      resolve(response);
    });
  });
};

export const loginUsuario = (usuario) => {
  return new Promise((resolve, eject) => {
    httpClient.post('/usuario/login',usuario).then((response) => {
      resolve(response);
    });
  });
};

export default resgistrarUsuario; 