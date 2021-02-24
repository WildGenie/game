import * as React from "react"
import { NavLink } from "react-router-dom"

import "./Footer.scss"
import FooterLogo from "./footer-logo.png"

const Footer: React.FunctionComponent = (): JSX.Element => {
	return (
		<footer className="app__footer footer">
			<div className="footer__widget">
				<img
					className="footer__logo"
					src={FooterLogo}
					alt="Bastion of Shadows logo"
				/>
			</div>
			<div className="footer__widget">
				<div>
					<p className="footer__paragraph">
						&copy; {new Date().getFullYear()} Bastion of Shadows. All rights reserved. The names of places, creatures, technologies, etc. are trademarks of Lucasfilm Ltd. and The Walt Disney Company. Bastion of Shadows does not own or lease the rights to these trademarks. Bastion of Shadows uses all trademarks in association with Fair Use Doctrine in the United States.
					</p>
					<p className="footer__paragraph">
						See our statement on the <NavLink to="/fair-use">Fair Use of intellectual property</NavLink>.
					</p>
				</div>
			</div>
		</footer>
	)
}

export default Footer