import * as React from "react"
import UserHome from "@/components/site/users/UserHome"
import { ApplicationUserModel } from "@/tools/definitions/users"
import { getDocumentTitle } from "@/tools/utils"

type HomeProps = {
	user: ApplicationUserModel
}

const Home: React.FunctionComponent<HomeProps> = ({ user }: HomeProps): JSX.Element => {

	document.title = getDocumentTitle(user?.userName)

	return (
		<UserHome identifier={`${user?.userName}'s`}/>
	)
}

export default Home