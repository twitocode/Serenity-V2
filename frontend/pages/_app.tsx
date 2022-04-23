import { ChakraProvider } from "@chakra-ui/react";
import type { AppProps } from "next/app";
import { Provider } from "react-redux";
import "../styles/globals.css";
import { theme } from "../styles/theme";
import { store } from "../lib/store";

function MyApp({ Component, pageProps }: AppProps) {
	return (
		<Provider store={store}>
			<ChakraProvider theme={theme}>
				<Component {...pageProps} />
			</ChakraProvider>
		</Provider>
	);
}

export default MyApp;
