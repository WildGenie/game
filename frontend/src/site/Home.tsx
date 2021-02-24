import * as React from "react"
import { getDocumentTitle } from "@/tools/utils"

const Home: React.FunctionComponent = (): JSX.Element => {

	document.title = getDocumentTitle("Home")

	return (
		<>
			Home
		</>
	)
}

export default Home