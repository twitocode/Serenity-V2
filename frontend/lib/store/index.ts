import { configureStore } from "@reduxjs/toolkit";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { commentsApi } from "./apis/commentsApi";
import { identityApi } from "./apis/identityApi";
import { postsApi } from "./apis/postsApi";
import counterSlice from "./slices/counterSlice";

export const store = configureStore({
	reducer: {
		counter: counterSlice,
		[identityApi.reducerPath]: identityApi.reducer,
		[postsApi.reducerPath]: postsApi.reducer,
		[commentsApi.reducerPath]: commentsApi.reducer,
	},
	middleware: (getDefaultMiddleware) =>
		getDefaultMiddleware()
			.concat(identityApi.middleware)
			.concat(commentsApi.middleware)
			.concat(postsApi.middleware),
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
