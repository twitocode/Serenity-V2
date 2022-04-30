import env from "@beam-australia/react-env";
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { LoginUserDto, Response } from "../../types/api";

export const identityApi = createApi({
	reducerPath: "identityApi",
	baseQuery: fetchBaseQuery({
		baseUrl: env("NEXT_PUBLIC_SERVER_URL") ?? "http://localhost:8000/auth",
		prepareHeaders: (headers) => {
			const token = localStorage.getItem("JWT_ACCESS_TOKEN");

			if (token) {
				headers.set("Authorization", `Bearer ${token}`);
			}

			return headers;
		},
	}),
	endpoints: (builder) => ({
		login: builder.mutation<Response, LoginUserDto>({
			query: (body) => ({
				url: "login",
				body,
				method: "POST",
			}),
		}),
		register: builder.mutation<Response, LoginUserDto>({
			query: (body) => ({
				url: "login",
				body,
				method: "POST",
			}),
		}),
		getUser: builder.query<Response, null>({
			query: () => "user",
		}),
	}),
});

export const { useLoginMutation, useGetUserQuery, useRegisterMutation } = identityApi;
