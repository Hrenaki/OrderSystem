import LoginRegisterPanel from './components/LoginRegister/LoginRegisterPanel';

import { UserState } from './common/UserState';
import MainPanel from './components/MainPanel/MainPanel';
import { useSelector } from 'react-redux';

function App() {
  const isLogged = useSelector<UserState, boolean>((x) => x.isLogged);
  console.log(isLogged);

  return (
    <div style={{ backgroundColor: '#202020' }}>
    <header>
      <nav className='navbar navbar-expand-sm navbar-toggleable-sm navbar-light border-bottom mb-3'>
        <div className='container-fluid'>OrderSystem</div>
      </nav>
    </header>
      <div className="container d-flex justify-content-center vh-100 pb-3">
        {isLogged ? <MainPanel /> : <LoginRegisterPanel backgroundColor={'#FFFFFF'} />}
      </div>
    </div>
  );
}

export default App;