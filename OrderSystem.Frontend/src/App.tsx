import LoginRegisterPanel from './components/LoginRegister/LoginRegisterPanel';

import { UserState, logout } from './common/UserState';
import MainPanel from './components/MainPanel/MainPanel';
import { useDispatch, useSelector } from 'react-redux';

function App() {
  const dispatch = useDispatch();
  const user = useSelector<UserState, UserState>((x) => x);
  const isLogged = user.isLogged;

  return (
    <div className='d-flex flex-column vh-100'>
      <header>
        <nav className='navbar navbar-expand-sm navbar-toggleable-sm navbar-light border-bottom mb-3'>
          <div className='container-fluid'>
            <a className="navbar-brand" href="#">OrderSystem</a>
          </div>
          {isLogged ?
            <div className='d-flex align-items-center'>
              <span className='text-nowrap m-2'>Account: {user.username}</span>
              <button className="btn btn-light m-2" onClick={() => dispatch(logout())}>Logout</button>
            </div>
            : null}
        </nav>
      </header>
      <div className="container flex-fill d-flex justify-content-center pb-3">
        {isLogged ? <MainPanel /> : <LoginRegisterPanel backgroundColor={'#FFFFFF'} />}
      </div>
    </div>
  );
}

export default App;