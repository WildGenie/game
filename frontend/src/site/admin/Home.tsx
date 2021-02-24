import * as React from "react"
import Navigation from "@/site/admin/Navigation"
import { getDocumentTitle } from "@/tools/utils"

const Home: React.FunctionComponent = (): JSX.Element => {

	document.title = getDocumentTitle("Admin")

	return (
		<section className="admin">
			<Navigation/>

			<section className="admin__content">
				<p>Select an administrator action to make changes.</p>
			</section>
		</section>
	)
}

export default Home