import * as React from "react"
import Navigation from "@/site/admin/Navigation"

const Home: React.FunctionComponent = (): JSX.Element => {
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