import React, { useState } from 'react';
import LoginRegisterPanel from './components/LoginRegister/LoginRegisterPanel';

import { logout, UserState } from './common/UserState';
import MainPanel from './components/MainPanel/MainPanel';
import { useDispatch, useSelector } from 'react-redux';

function App() {
  const isLogged = useSelector<UserState, boolean>((x) => x.isLogged);
  console.log(isLogged);

  return (
    <div className='vh-100' style={{ backgroundColor: '#202020' }}>
      <div className="container d-flex justify-content-center h-100">
        {isLogged ? <MainPanel /> : <LoginRegisterPanel backgroundColor={'#FFFFFF'} />}
      </div>
    </div>
  );
}

export default App;