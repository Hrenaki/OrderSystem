import { configureStore } from "@reduxjs/toolkit";

import Reducer from "./common/UserState";

const store = configureStore({
	reducer: Reducer,
	devTools: true,
	//preloadedState: persistedState -- move to redux-persist
});

export default store;