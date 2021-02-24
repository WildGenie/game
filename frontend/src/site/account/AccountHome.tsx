import * as React from "react"
import UserHome from "@/components/site/users/UserHome"
import { getDocumentTitle } from "@/tools/utils"

const AccountHome: React.FunctionComponent = () => {

	document.title = getDocumentTitle("My account")

	return (
		<UserHome identifier="your"/>
	)
}

export default AccountHome