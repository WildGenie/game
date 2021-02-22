import "regenerator-runtime/runtime"
import * as React from "react"
import { render } from "react-dom"
import { BrowserRouter} from "react-router-dom"
import { Provider } from "react-redux"
import App from "@/site/App"
import store from "@/site/store"

render(
	<BrowserRouter>
		<Provider store={store}>
			<App/>
		</Provider>
	</BrowserRouter>,
	document.getElementById("root")
)