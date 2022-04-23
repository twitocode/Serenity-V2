import env from "@beam-australia/react-env";
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

// Define a service using a base URL and expected endpoints
export const authApi = createApi({
	reducerPath: "authApi",
	baseQuery: fetchBaseQuery({ baseUrl: env("NEXT_PUBLIC_SERVER_URL") ?? "http://localhost:8000" }),
	endpoints: (builder) => ({
		login: builder.query<string, string>({
			query: () => `auth/login`,
		}),
	}),
});

export const { useLoginQuery } = authApi;