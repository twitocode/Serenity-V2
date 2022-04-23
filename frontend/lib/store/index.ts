import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { authApi } from "./apis/authApi";
import counterSlice from "./slices/counterSlice";

export const store = configureStore({
	reducer: {
		counter: counterSlice,
		[authApi.reducerPath]: authApi.reducer,
	},
	middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(authApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
