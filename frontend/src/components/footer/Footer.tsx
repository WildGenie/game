import * as React from "react"

import "./Footer.scss"

const Footer: React.FunctionComponent = (): JSX.Element => {
	return (
		<footer className="app__footer footer">
			&copy; {new Date().getFullYear()} $$website_name$$. All rights reserved.
		</footer>
	)
}

export default Footer